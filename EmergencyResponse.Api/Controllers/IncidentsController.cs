using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmergencyResponse.Api.Data;
using EmergencyResponse.Api.Models;
using EmergencyResponse.Api.Services;

namespace EmergencyResponse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GeminiService _geminiService;

        public IncidentsController(AppDbContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        // GET: api/incidents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        // GET: api/incidents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            return incident;
        }

        // POST: api/incidents
        [HttpPost]
        public async Task<ActionResult<Incident>> CreateIncident(Incident incident)
        {
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
        }

        // PUT: api/incidents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncident(int id, Incident incident)
        {
            if (id != incident.Id)
            {
                return BadRequest();
            }

            _context.Entry(incident).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/incidents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // POST: api/incidents/5/assign-unit
        [HttpPost("{id}/assign-unit")]
        public async Task<IActionResult> AssignUnit(int id, [FromBody] AssignUnitRequest request)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound("Incident not found.");
            }

            var unit = await _context.Units.FindAsync(request.UnitId);
            if (unit == null)
            {
                return NotFound("Unit not found.");
            }

            var incidentUnit = new IncidentUnit
            {
                IncidentId = id,
                UnitId = request.UnitId
            };

            _context.IncidentUnits.Add(incidentUnit);

            unit.Status = UnitStatus.Busy;

            await _context.SaveChangesAsync();

            return Ok(incidentUnit);
        }
        // POST: api/incidents/analyze
        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeIncident([FromBody] AnalyzeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RawText))
            {
                return BadRequest("Text is required.");
            }

            var aiResponse = await _geminiService.AnalyzeIncidentTextAsync(request.RawText);

            return Ok(new { rawAiResponse = aiResponse });
        }
    }
    public class AssignUnitRequest
    {
        public int UnitId { get; set; }
    }

    public class AnalyzeRequest
    {
        public string RawText { get; set; } = string.Empty;
    }
}