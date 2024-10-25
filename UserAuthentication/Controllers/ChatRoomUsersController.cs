using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomUsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChatRoomUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatRoomUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatRoomUser>>> GetChatRoomUser()
        {
            return await _context.ChatRoomUser.ToListAsync();
        }

        // GET: api/ChatRoomUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatRoomUser>> GetChatRoomUser(long id)
        {
            var chatRoomUser = await _context.ChatRoomUser.FindAsync(id);

            if (chatRoomUser == null)
            {
                return NotFound();
            }

            return chatRoomUser;
        }

        // PUT: api/ChatRoomUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatRoomUser(long id, ChatRoomUser chatRoomUser)
        {
            if (id != chatRoomUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatRoomUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatRoomUserExists(id))
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

        // POST: api/ChatRoomUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChatRoomUser>> PostChatRoomUser(ChatRoomUser chatRoomUser)
        {
            _context.ChatRoomUser.Add(chatRoomUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatRoomUser", new { id = chatRoomUser.Id }, chatRoomUser);
        }

        // DELETE: api/ChatRoomUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatRoomUser(long id)
        {
            var chatRoomUser = await _context.ChatRoomUser.FindAsync(id);
            if (chatRoomUser == null)
            {
                return NotFound();
            }

            _context.ChatRoomUser.Remove(chatRoomUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChatRoomUserExists(long id)
        {
            return _context.ChatRoomUser.Any(e => e.Id == id);
        }
    }
}
