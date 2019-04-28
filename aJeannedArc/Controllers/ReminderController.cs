﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aJeannedArc.Models;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace aJeannedArc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : Controller
    {
        private readonly ReminderContext _context;

        public ReminderController(ReminderContext context)
        {
            _context = context;

            if (_context.Reminders.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.Reminders.Add(new Reminder { Title = "Reminder1" });
                _context.SaveChanges();
            }
        }

        // GET: api/Reminder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders()
        {
            return await _context.Reminders.ToListAsync();
        }

        // GET: api/Reminder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reminder>> GetReminder(long id)
        {
            var reminder = await _context.Reminders.FindAsync(id);

            if (reminder == null)
            {
                return NotFound();
            }

            return reminder;
        }

        // POST: api/Reminder/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Reminder>> CreateReminder(Reminder reminder)
        {
            Debug.WriteLine(reminder);
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReminder), new { id = reminder.Id }, reminder);
        }

        // PUT: api/Reminder/update/5
        [HttpPost("update/{id}")]
        public ActionResult<Reminder> Put(long id, [FromBody]Reminder reminder)
        {
            var reminderBdd = _context.Reminders.First(app => app.Id == id);

            if (reminderBdd == null)
                return NoContent();
            
            reminderBdd.Title = reminder.Title;
            reminderBdd.Date = reminder.Date;
            reminderBdd.UserId = reminder.UserId;
            reminderBdd.IsFinished = reminder.IsFinished;
            reminder.Id = reminderBdd.Id;

            _context.SaveChanges();

            return reminder;
        }

        // Get for user : api/Reminder/ForUser/id
        [HttpGet("ForUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetReminderForUser(long userId)
        {
            var reminders = _context.Reminders.Where(a => a.UserId == userId).ToList();

            if (reminders == null)
            {
                return NotFound();
            }

            return reminders;
        }
    }
}
