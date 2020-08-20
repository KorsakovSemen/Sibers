using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Models
{
    public class ProjectDTOListViewModels
    {
        private static readonly IList<string> priority = new List<string> { "Все", ">=", "<=", "=", };
        public ProjectDTOListViewModels()
        {
            Priorities = new SelectList(priority);
        }

        public IEnumerable<ProjectDTO> Projects { get; set; }
        public SelectList Priorities { get; set; }

        /// <summary>
        /// Применение фильтров
        /// </summary>
        /// <param name="projectDTOs">все проекты</param>
        /// <param name="priorityFilter">выбранный фильтр из Priorities</param>
        /// <param name="priorityCurrent">выбранное значение приоритета</param>
        public void ApplyFilters(IEnumerable<ProjectDTO> projectDTOs, string priorityFilter, int priorityCurrent, DateTime? startDateTimeFrom, DateTime? startDateTimeTo)
        {
            if (projectDTOs != null)
            {
                Projects = projectDTOs;
                FilterPriority(projectDTOs, priorityFilter, priorityCurrent);
                FilterStartDate(startDateTimeFrom, startDateTimeTo);
            }
        }
        // фильтр приоритета
        private void FilterPriority(IEnumerable<ProjectDTO> projectDTOs, string priorityFilter, int priorityCurrent)
        {
            if (priorityCurrent < 0) priorityCurrent = 0;
            if (priorityFilter != null)
            {
                switch (priorityFilter)
                {
                    case "Все":
                        Projects = projectDTOs;
                        break;
                    case ">=":
                        Projects = Projects.Where(item => item.Priority >= priorityCurrent);
                        break;
                    case "<=":
                        Projects = Projects.Where(item => item.Priority <= priorityCurrent);
                        break;
                    case "=":
                        Projects = Projects.Where(item => item.Priority == priorityCurrent);
                        break;
                }
            }
        }
        // фильтр даты начала
        private void FilterStartDate(DateTime? startDateTimeFrom, DateTime? startDateTimeTo)
        {
            if (startDateTimeFrom != null && startDateTimeTo != null)
                Projects = Projects.Where(item => item.StartDate >= startDateTimeFrom && item.StartDate <= startDateTimeTo);
            else if (startDateTimeFrom != null || startDateTimeTo != null)
            {
                IEnumerable<ProjectDTO> startDateTimeFromList = new List<ProjectDTO>();
                IEnumerable<ProjectDTO> startDateTimeToList = new List<ProjectDTO>();
                List<ProjectDTO> dateTimeList = new List<ProjectDTO>();
                if (startDateTimeFrom != null) startDateTimeFromList = Projects.Where(item => item.StartDate >= startDateTimeFrom);
                if (startDateTimeTo != null) startDateTimeFromList = Projects.Where(item => item.StartDate <= startDateTimeTo);
                dateTimeList.AddRange(startDateTimeFromList);
                dateTimeList.AddRange(startDateTimeToList);
                Projects = dateTimeList.Distinct();
            }
        }
    }
}