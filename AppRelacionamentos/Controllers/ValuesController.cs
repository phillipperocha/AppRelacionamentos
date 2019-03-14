using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRelacionamentos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppRelacionamentos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext context;

        public ValuesController(DataContext context)
        {
            this.context = context;
        }
        // GET api/values
        [HttpGet]
        // Primeiro dizemos ao método que ele deve ser assíncrono
        // E daí retornamos uma tarefa (Task) de IActionResult, em vez do próprio IActionResult
        // Uma Task representa uma operação assíncrona que pode retornar um valor
        // O que isse quer dizer é que qualquer coisa que estamos esperando dentro de nosso método
        // Manterá a nossa Thread aberta e não bloqueará as requisições enquanto espera pela resposta.
        public async Task<IActionResult> GetValues()
        {
            // Agora aqui no método, nós precisaremos informar que ele deverá esperar por essa resposta
            // Para isso utilizaremos a palavra-chave "await". Mas daí teremos que usar uma
            // versão async da nossa ToList(). Agora esse método é um método assíncrono.
            var values = await this.context.Values.ToListAsync();

            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await this.context.Values.FirstOrDefaultAsync(v => v.Id == id);

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
