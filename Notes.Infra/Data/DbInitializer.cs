using Domain.Entities;
using Notes.Application.Interfaces;

namespace Notes.Infra.Data
{
    public class DbInitializer
    {
        private readonly IAppDbContext _context;

        public DbInitializer(IAppDbContext context) => 
            _context = context;

        public void Seed()
        {
            SeedUser();
            SeedNotes();
        }

        private void SeedUser()
        {
            if (_context.Users.Any()) return;

            var appUserId = "test";
            _context.Users.Add(new User 
            { 
                AppUserId = appUserId,
                Email = "test@test.com",
                FirstName = "Tester"
            });

            _context.SaveChanges();
        }

        private void SeedNotes()
        {
            if (_context.Notes.Any()) return;

            var notes = SeedHelper.SeedData<Note>("Notes.json");

            if (notes == null) return;

            _context.Notes.AddRange(notes);
            _context.SaveChanges();   
        }
    }
}
