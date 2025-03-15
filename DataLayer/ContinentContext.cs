﻿using DataLayer.BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal class ContinentContext
    {
        private FootballteamsContext dbContext;

        public ContinentContext(FootballteamsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Continent Read(string key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Continent> query = dbContext.Continents;

            if (useNavigationalProperties)
            {
                query = query.Include(c => c.ContinentCode);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault(c => c.ContinentCode == key);
        }
        public List<Continent> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Continent> query = dbContext.Continents;

            if (useNavigationalProperties)
            {
                query = query.Include(c => c.ContinentCode);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }
        public void Update(Continent item, bool useNavigationalProperties = false)
        {
            Continent existing = Read(item.ContinentCode, useNavigationalProperties);
            if (existing == null)
            {
                throw new ArgumentException("Continent not found!");
            }

            dbContext.Entry(existing).CurrentValues.SetValues(item);
            dbContext.SaveChanges();
        }

        public void Delete(string key)
        {
            Continent existing = Read(key);
            if (existing == null)
            {
                throw new ArgumentException("Continent not found!");
            }

            dbContext.Continents.Remove(existing);
            dbContext.SaveChanges();
        }
    }
}
