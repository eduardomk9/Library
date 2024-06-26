﻿using Core.Business;
using Core.DTOs.Book;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Authorize("Bearer")]
    [Authorize(AuthenticationSchemes = "CustomAuth")]
    [Route("[controller]")]
    [Tags("Authenticação")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookBusiness _bookBusiness;

        // Corrigindo a definição do construtor
        public BookController(IBookBusiness bookBusiness)
        {
            _bookBusiness = bookBusiness;
        }

        /// <summary>
        /// GetBooks
        /// </summary>
        /// <remarks>
        /// This dont method allow anonymous.
        /// 
        /// You have to call this method with a token in the header.
        /// 
        /// You can call this method to get books.
        /// 
        /// Fill correctly all parameters to call this method.
        /// 
        /// None of the parameters are required to call this methode
        /// </remarks>
        /// <param name="model">Modelo de entrada</param>
        [HttpPost("GetBooks")]
        [SwaggerResponse(200, "Informações", typeof(IEnumerable<BookModel>))]
        [SwaggerResponse(400, "Erro", typeof(string))]
        public async Task<IActionResult> GetBooksAsync([FromBody] GetBooksDto model)
        {
            try
            {
                IEnumerable<BookModel> books = await _bookBusiness.GetBooksAsync(model);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest($"BookController | GetBooksAsync | {ex.Message}");
            }
        }
    }
}
