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
            SeedNotes();
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
