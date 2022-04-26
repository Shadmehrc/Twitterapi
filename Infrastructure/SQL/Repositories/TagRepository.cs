using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.RepositoryInterfaces;
using Core.Entities;
using Core.Models;
using Infrastructure.SQL.Context;


namespace Infrastructure.SQL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DatabaseContext _context;

        public TagRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public Task<Tag> FindHelper(int id)
        {
            var result = _context.Tags.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }
        public async Task<bool> Create(string word)
        {
            var tag = new Tag() { Word = word };
            await _context.AddAsync(tag);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Tag> Get(int id)
        {
            return await FindHelper(id);
        }

        public async Task<bool> Edit(EditTweetDTO model)
        {
            var tag = await FindHelper(model.Id);
            if (tag != null)
            {
                tag.Word = model.Word;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> Delete(int id)
        {
            var tag = await FindHelper(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
