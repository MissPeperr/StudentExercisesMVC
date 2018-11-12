using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using StudentExercisesAPI.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVC.Models.ViewModels
{
    public class StudentCohortViewModel
    {
        // what I want to see on my 'Create Student' view
        public Student student { get; set; }
        public List<SelectListItem> Cohorts { get; set; }

        // create an empty constructor for the ViewModel
        public StudentCohortViewModel() { }

        // create a constructor for the ViewModel so the VM knows what to do with the data
        public StudentCohortViewModel(IConfiguration config){
            using (IDbConnection conn = new SqlConnection(config.GetConnectionString("DefaultConnection")))
            {
                // using the connection string we have in 'appsettings'
                // I want to query the DB for Cohorts
                Cohorts = conn.Query<Cohort>(@"
                    SELECT
                        Id,
                        Name
                    FROM Cohort
                    ")
                    // for each list item (cohort), create a new SelectListItem with the properties:
                    // Text = cohort.Name
                    // Value = cohort.Id (and convert that one into a string because the 'Value' property is of type string)
                    .Select(li => new SelectListItem
                    {
                        Text = li.Name,
                        Value = li.Id.ToString()
                    }).ToList();

            }
            // insert a new SelectListItem at index 0
            Cohorts.Insert(0, new SelectListItem
            {
                // set the 'Text' property of the SelectListItem to "Choose a Cohort"
                // set the 'Value' property of the SelectListItem to "0"
                // This is also *technically* the Id of this "Cohort" called "Choose a Cohort"
                Text = "Choose a Cohort",
                Value = "0"
            });

        }
    }
}
