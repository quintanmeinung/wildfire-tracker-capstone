using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.DAL.Concrete;
using System.Linq.Expressions;

namespace project_wildfire_web.DAL.Concrete;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly FireDataDbContext _context;

    public UserPreferencesRepository(FireDataDbContext context )
    {
        _context = context;
    }

}
