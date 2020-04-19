using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Controllers
{
    // Ten adres będzie nas kierował do kontrolera ValuesController, wykonuje się get
    // http://localhost:5000/api/Values
    // Jeśli chcemy coś pobrać to np:
    // GET http://localhost:5000/api/Values
    // lub konkretne ID
    // GET http://localhost:5000/api/Values/100
    // lub żądanie POST (dodanie jakiejś wartości)
    // POST http://localhost:5000/api/Values
    // itd. PUT (modyfikacja), DELETE (kasowanie)
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
        //public ActionResult<IEnumerable<string>> Get()
        // Zmieniamy na IActionResult, bo zwracamy ok, a nie listę
        public async Task<IActionResult> GetValues()
        {
            // throw new Exception("Generujemy wyjątek");
            //return new string[] { "value1", "value2" };
            // zwracamy wszystkie wartości
            var values = await this.context.Values.ToListAsync();
            // zwracamy status OK wraz ze wszystkimi wartościami
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        public async Task<IActionResult> GetValue(int id)
        {
            // zwracamy wartość po id, dlatego jest x.id, jeśli inaczej to trzeba zmienić x.id
            // Można zamiast First wybrać FirstOrDefault. Metoda First zwróci wyjątek jeśli nie znajdzie wartości. FirstOrDefault nie. Nie wiem tylko co zwróci jak nie znajdzie? NULL?
            // FirstOrDefault jednak zwróci "204 no content", Find zwraca też 204 no content, First zwróci "500 Internal Server Error" czyli niejasny komunikat.
            var value = await this.context.Values.FirstOrDefaultAsync(x => x.id == id);
            // zwracamy status OK wraz ze wszystkimi wartościami
            return Ok(value);
        }

        // POST api/values
        // Służy do dodawania danych
        [HttpPost]
        //public void AddValue([FromBody] Value value)
        // Zamiast void dajemy sobie IActionResult, żeby móc zrobić status OK
        public async Task<IActionResult> AddValue([FromBody] Value value)
        {
            this.context.Values.Add(value);
            // Metoda, która zapisze nasze zmiany do bazy danych
            await this.context.SaveChangesAsync();
            return Ok(value);
        }

        // PUT api/values/5
        // Służy do edycji danych
        [HttpPut("{id}")]
        //public void EditValue(int id, [FromBody] Value value)
        // Zamiast void dajemy sobie IActionResult, żeby móc zrobić status OK
        public async Task<IActionResult> EditValue(int id, [FromBody] Value value)
        {
            // Znajdujemy najpierw wartość o podanym id. Nie wiem czy metody find nie lepiej użyć powyżej dla GetValue? Zwróci nam prawidłowo nulla przecież, a nie wartość domyślną.
            // Nie lepiej, bo // FirstOrDefault jednak zwróci "204 no content", Find zwraca też 204 no content, First zwróci "500 Internal Server Error" czyli niejasny komunikat.
            var data = await this.context.Values.FindAsync(id);
            //var oldData = data;
            data.Name = value.Name;
            this.context.Update(data);
            await this.context.SaveChangesAsync();
            //return Ok("OldData: " + oldData + " NewData: " + data); <-- tak nie zadziała niestety, bo zwracać musi json, wypisze "data.value" zamiast json
            return Ok(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        //public void DeleteValue(int id)
        // Zamiast void dajemy sobie IActionResult, żeby móc zrobić status OK
        public async Task<IActionResult> DeleteValue(int id)
        {
            var data = await this.context.Values.FindAsync(id);
            //var oldData = data;
            // Zabezpieczenie przed zwróceniem Internal server error. Zwróci nam dzięki temu 204 No Content.
            if (data == null)
                return NoContent();

            this.context.Remove(data);
            await this.context.SaveChangesAsync();
            //return Ok("OldData: " + oldData + " NewData: " + data); <-- tak nie zadziała niestety, bo zwracać musi json, wypisze "data.value" zamiast json
            return Ok(data);
        }
    }
}
