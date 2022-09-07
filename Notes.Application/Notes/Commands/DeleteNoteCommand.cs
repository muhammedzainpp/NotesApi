using MediatR;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands
{
    public class DeleteNoteCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
    public class DeleteCommandHandler : IRequestHandler<DeleteNoteCommand, int>
    {
        private readonly IAppDbContext _context;
        public DeleteCommandHandler(IAppDbContext context) => _context = context;
        public async Task<int> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {

            var result = await _context.Notes.FindAsync(request.Id);


            if (result == null)
                throw new NotFoundException($"Note not found with id {request.Id}");

            _context.Notes.Remove(result);
            await _context.SaveChangesAsync();
            return result.Id;
        }
    }

}
