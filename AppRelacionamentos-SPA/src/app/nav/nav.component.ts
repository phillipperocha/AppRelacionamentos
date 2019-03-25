import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  // Agora nós vamos injetar o serviço Auth dentro do nosso construtor
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      console.log('Logged in successfully');
    }, error => {
        console.log('Failed to login');
    });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    // As duas exclamações dizem que vai retornar um boolean, true ou false, se tiver algo dentro do token.
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out');
  }
}
