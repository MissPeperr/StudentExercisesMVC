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

namespace StudentExerciseMVC.Controllers
{
    public class CohortsController : Controller
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
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
        // GET: Cohorts
        public async Task<ActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {
                // I want EVERYTHING about ALL of the cohorts
                IEnumerable<Cohort> cohorts = await conn.QueryAsync<Cohort>(@"
                    SELECT 
                        c.Id,
                        c.Name
                    FROM Cohort c
                ");
                return View(cohorts);
            }
        }

        // GET: Cohorts/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // I want EVERYTHING about ONE cohort
            string sql = $@"
            SELECT
                c.Id,
                c.Name
            FROM Cohort c
            WHERE c.Id = {id}
            ";

            using (IDbConnection conn = Connection)
            {
                Cohort cohorts = await conn.QueryFirstAsync<Cohort>(sql);
                return View(cohorts);
            }
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cohorts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cohorts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}