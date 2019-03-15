import { Component } from '@angular/core';

@Component({
  // esse componente é uma tag denrto da página HTML
  selector: 'app-root',
  // É a página que será renderizada nessa rota
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // As interpolações que serão acessadas na views como {{ title }}
  title = 'AppRelacionamentos-SPA';
}
