using Microsoft.AspNetCore.Mvc;
using TrustVault_backend.Entity;
using TrustVault_backend.Services.Interface;
using Microsoft.AspNetCore.Mvc;


namespace TrustVault_backend.Controllers
{
    [ApiController]
    [Route("ContactUs")]
    public class ContactUsController : Controller
    {
        private readonly IContactFormService _contactFormService;

        public ContactUsController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }

        [HttpPost("SubmitContactForm")]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                await _contactFormService.SubmitContactFormAsync(contactForm);
                return Ok(new { message = "Your message has been submitted successfully!" });
            }
            return BadRequest(new { message = "Failed to submit the form. Please try again." });
        }
    }
}




