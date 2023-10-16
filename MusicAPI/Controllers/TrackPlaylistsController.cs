using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Data.Entities;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrackPlaylistsController : ControllerBase
    {
        private readonly MusicAPIContext _context;

        public TrackPlaylistsController(MusicAPIContext context)
        {
            _context = context;
        }

        // GET: api/TrackPlaylists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackPlaylist>>> GetTrackPlaylist()
        {
          if (_context.TrackPlaylist == null)
          {
              return NotFound();
          }
            return await _context.TrackPlaylist.ToListAsync();
        }

        // GET: api/TrackPlaylists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackPlaylist>> GetTrackPlaylist(int id)
        {
          if (_context.TrackPlaylist == null)
          {
              return NotFound();
          }
            var trackPlaylist = await _context.TrackPlaylist.FindAsync(id);

            if (trackPlaylist == null)
            {
                return NotFound();
            }

            return trackPlaylist;
        }

        // PUT: api/TrackPlaylists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrackPlaylist(int id, TrackPlaylist trackPlaylist)
        {
            if (id != trackPlaylist.TrackId)
            {
                return BadRequest();
            }

            _context.Entry(trackPlaylist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackPlaylistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TrackPlaylists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackPlaylist>> PostTrackPlaylist(TrackPlaylist trackPlaylist)
        {
          if (_context.TrackPlaylist == null)
          {
              return Problem("Entity set 'MusicAPIContext.TrackPlaylist'  is null.");
          }
            _context.TrackPlaylist.Add(trackPlaylist);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TrackPlaylistExists(trackPlaylist.TrackId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTrackPlaylist", new { id = trackPlaylist.TrackId }, trackPlaylist);
        }

        // DELETE: api/TrackPlaylists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrackPlaylist(int id)
        {
            if (_context.TrackPlaylist == null)
            {
                return NotFound();
            }
            var trackPlaylist = await _context.TrackPlaylist.FindAsync(id);
            if (trackPlaylist == null)
            {
                return NotFound();
            }

            _context.TrackPlaylist.Remove(trackPlaylist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackPlaylistExists(int id)
        {
            return (_context.TrackPlaylist?.Any(e => e.TrackId == id)).GetValueOrDefault();
        }
    }
}
