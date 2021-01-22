using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Praca_dyplomowa.Context;
using Praca_dyplomowa.Entities;
using Praca_dyplomowa.Models;

namespace Praca_dyplomowa.Services
{
    public interface IActivityService
    {
        List<TrainingJSON> GetTrainings(User CurrentUser, PageJSON page);
        TrainingJSON GetTrainingDetails(User CurrentUser, DetailsIdJSON trainingId);
        bool AddTraining(User CurrentUser, NewTrainingJSON newTraining);
        bool EditTraining(User CurrentUser, EditTrainingJSON editTraining);
        bool DeleteTraining(User CurrentUser, RemoveIdJSON id);
    }
    public class ActivityService : IActivityService
    {
        private ProgramContext _context;
        private IMapper _mapper;

        public ActivityService(ProgramContext programContext, IMapper mapper)
        {
            _context = programContext;
            _mapper = mapper;
        }

        public bool AddTraining(User CurrentUser, NewTrainingJSON newTraining)
        {
            var ifTrainingExist = _context.Trainings
                .Count(t => t.StartTime.Equals(newTraining.StartTime));

            var ifRouteExist = _context.Routes
                .Count(r => r.RouteId == newTraining.RouteId && r.Place.Region.UserId == CurrentUser.UserId);

            if (ifTrainingExist == 0 && ifRouteExist == 1)
            {
                try
                {
                    var route = _context.Routes
                        .FirstOrDefault(r => r.RouteId == newTraining.RouteId && r.Place.Region.UserId == CurrentUser.UserId);

                    var tempTraining = _mapper.Map<Training>(newTraining);
                    tempTraining.UserId = CurrentUser.UserId;
                    tempTraining.TrainingType = route.RouteType;
                    if (newTraining.Distance == 0)
                        if (route.RouteType == 1)
                            tempTraining.Distance = route.Length;
                        else
                            tempTraining.Distance = route.HeightDifference;

                    _context.Trainings.Add(tempTraining);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool DeleteTraining(User CurrentUser, RemoveIdJSON id)
        {
            var userTraining = _context.Trainings
                .FirstOrDefault(t => t.User.UserId == CurrentUser.UserId && t.TrainingId == id.Id);

            if (userTraining != null)
            {
                try
                {
                    _context.Trainings.Remove(userTraining);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool EditTraining(User CurrentUser, EditTrainingJSON editTraining)
        {
            var userTraining = _context.Trainings
                .FirstOrDefault(t => t.User.UserId == CurrentUser.UserId && t.TrainingId == editTraining.TrainingId);

            if (userTraining != null)
            {
                try
                {
                    userTraining.TrainingType = editTraining.TrainingType;
                    userTraining.TrainingName = editTraining.TrainingName;
                    userTraining.TrainingDescription = editTraining.TrainingDescription;
                    userTraining.StartTime = editTraining.StartTime;
                    userTraining.EndTime = editTraining.EndTime;
                    userTraining.ActivityTime = editTraining.ActivityTime;
                    userTraining.Distance = editTraining.Distance;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public TrainingJSON GetTrainingDetails(User CurrentUser, DetailsIdJSON trainingId)
        {
            var userTrainings = _context.Trainings
                .FirstOrDefault(t => t.User.UserId == CurrentUser.UserId && t.TrainingId == trainingId.Id);

            if (userTrainings == null)
                return null;
            return _mapper.Map<TrainingJSON>(userTrainings);
        }

        public List<TrainingJSON> GetTrainings(User CurrentUser, PageJSON page)
        {
            var userTrainings = _context.Trainings
                .Include(r => r.Route)
                .Where(t => t.User.UserId == CurrentUser.UserId)
                .OrderByDescending(d => d.TrainingId)
                .Skip((page.Page - 1) * page.Number)
                .Take(page.Number);



            return _mapper.Map<List<TrainingJSON>>(userTrainings);
        }
    }
}
