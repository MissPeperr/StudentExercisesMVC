using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesAPI.Data;
using StudentExerciseMVC.Models.ViewModels;

namespace StudentExerciseMVC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Instructors
        public async Task<ActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {
            // I want EVERYTHING about ALL of the instructors
                IEnumerable<Instructor> instructors = await conn.QueryAsync<Instructor>(@"
                    SELECT 
                        i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId
                    FROM Instructor i
                ");
                return View(instructors);
            }
        }

        // GET: Instructors/Details/5
        public async Task<ActionResult> Details(int id)
        {
        // I want EVERYTHING about ONE instructor
            string sql = $@"
            SELECT
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.Specialty,
                i.CohortId
            FROM Instructor i
            WHERE i.Id = {id}
            ";

            using (IDbConnection conn = Connection)
            {
                Instructor instructors = await conn.QueryFirstAsync<Instructor>(sql);
                return View(instructors);
            }
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            var model = new InstructorCreateViewModel(_config);
            return View(model);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InstructorCreateViewModel model)
        {
                // TODO: Add insert logic here
            string sql = $@"
                INSERT INTO Instructor
                (FirstName, LastName, SlackHandle, Specialty, CohortId)
                VALUES
                (
                    '{model.instructor.FirstName}', 
                    '{model.instructor.LastName}', 
                    '{model.instructor.SlackHandle}', 
                    '{model.instructor.Specialty}', 
                    {model.instructor.CohortId}
                 )";
            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Instructors/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // I want EVERYTHING about the ONE instructor on a new view (Instructors/Edit/5)
            string sql = $@"
            SELECT
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.Specialty,
                i.CohortId
            FROM Instructor i
            WHERE i.Id = {id}
            ";

            using (IDbConnection conn = Connection)
            {
                Instructor instructors = await conn.QueryFirstAsync<Instructor>(sql);
                return View(instructors);
            }
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Instructor instructor)
        {
            // UPDATING the DB with whatever was left in those fields on the edit view
            // If FirstName was NOT changed, it'll still update the DB with whatever was in there
            try
            {
                // TODO: Add update logic here
                string sql = $@"
                    UPDATE Instructor
                    SET FirstName = '{instructor.FirstName}',
                        LastName = '{instructor.LastName}',
                        SlackHandle = '{instructor.SlackHandle}',
                        Specialty = '{instructor.Specialty}'
                    WHERE Id = {id}";

                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return BadRequest();

                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Delete/5
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            // I want EVERYTHING about the instructor so I can populate the new view with its information
            string sql = $@"
            SELECT
                i.Id,
                i.FirstName,
                i.LastName,
                i.SlackHandle,
                i.Specialty,
                i.CohortId
            FROM Instructor i
            WHERE id = {id}
            ";
            using (IDbConnection conn = Connection)
            {
                Instructor instructor = await conn.QueryFirstAsync<Instructor>(sql);
                return View(instructor);
            }
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Instructor instructor)
        {
            string sql = $@"DELETE FROM Instructor WHERE Id = {id}";

            using (IDbConnection conn = Connection)
            {
                int rowsAffected = await conn.ExecuteAsync(sql);
                if (rowsAffected > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                throw new Exception("No rows affected");
            }
        }
    }
}