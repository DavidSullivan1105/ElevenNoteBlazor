﻿using DocumentFormat.OpenXml.Bibliography;
using ElevenNoteWebApp.Server.Data;
using ElevenNoteWebApp.Shared.Models.Note;
using Microsoft.EntityFrameworkCore;

namespace ElevenNoteWebApp.Server.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        public NoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        private string _userId;

        public async Task<bool> CreateNoteAsync(NoteCreate model)
        {
            var noteEntity = new Note
            {
                Title = model.Title,
                Content = model.Content,
                OwnerId = _userId,
                CreatedUtc = DateTimeOffset.UtcNow,
                CreatedUtc = model.CategoryId,

            };
            _context.Notes.Add(noteEntity);
            var numberOfChanges = await _context.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        public Task<bool> DeleteNoteAsync(int noteId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNoteAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NoteListItem>> GetAllNotesAsync()
        {
            var noteQuery = _context
                 .Notes
                 .Where(n => n.OwnerId == _userId)
                 .Select(n => new NoteListItem
                 {
                     Id = n.Id,
                     Title = n.Title,
                     CategoryName = n.Category.Name,
                     CreatedUtc = n.CreatedUtc,
                 });
            return await noteQuery.ToListAsync();
        }

        public async Task<NoteDetail> GetNoteByIdAsync(int noteId)
        {
            var noteEntity = await _context
                .Notes
                .Include(nameof(Category))
                .FirstOrDefaultAsync(n => n.Id == noteId && n.OwnerId == _userId);
            if (noteEntity == null)
                return null;

            var detail = new NoteDetail
            {
                Id = noteEntity.Id,
                Title = noteEntity.Title,
                Content = noteEntity.Content,
                CreatedUtc = noteEntity.CreatedUtc,
                ModifiedUtc = noteEntity.ModifiedUtc,
                CategoryName = noteEntity.Category.Name,
                CategoryId = noteEntity.Category.Id,
            };
            return detail;
        }

        public void SetUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNoteAsync(NoteEdit model)
        {
            throw new NotImplementedException();
        }

        
    }
}