import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  // Ele automaticamente nos dá o seletor 'app-value' e gera nosso template e css
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  // Declaramos uma propriedade sem tipo (como são as variáveis em javascript), mas não é assim que faremos depois
  values: any;

  // O Angular tem a injeção de serviços parecida com o ASP.NET, temos que injetar o serviço
  // no nosso construtor para que isso possa ser usado em nossa classe.

  
  constructor(private http: HttpClient) {
    // Cuidado com a importação automática aqui! Faça a importação do HttpClient pelo ANGULAR e não pelo SELENIUN

    
   }

  ngOnInit() {
    // O que será chamado quando o componente for inicializado. Acontece logo após o construtor.
    this.getValues();
  }
  
  // Faremos um método chamado GetValues

  getValues() {
    // Aqui usaremos http que injvetamos, e podemos ver os serviços disponíveis para usarmos.
    // E podemos escolher o verbo que iremos utilizar para a requisição, no nosso caso o GET.

    // O primeiro parâmetro é a URL
    // Para pear o conteúdo que vem do método get(), temos que usar o subscribe para pegar a resposta.
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      // O que faremos com a resposta quando ela vier

      this.values = response;
    }, error => {
      // O que faremos em caso de erro
      console.log(error);
    })

  }
}
