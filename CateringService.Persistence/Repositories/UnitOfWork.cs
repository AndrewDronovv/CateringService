﻿using CateringService.Domain.Repositories;

namespace CateringService.Persistence.Repositories;

public sealed class UnitOfWork : IUnitOfWorkRepository
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int SaveChanges()
    {
        if (_context.ChangeTracker.HasChanges())
        {
            return _context.SaveChanges();
        }
        return 0;
    }

    public async Task<int> SaveChangesAsync()
    {
        if (_context.ChangeTracker.HasChanges())
        {
            return await _context.SaveChangesAsync();
        }
        return 0;
    }
}