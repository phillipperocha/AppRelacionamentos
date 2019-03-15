import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
// Aqui a importação do HttpClientModule para consumirmos a API
import { HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
// Precisamos importar os componentes quando gerados como aqui
import { ValueComponent } from './value/value.component';

@NgModule({
   declarations: [
      AppComponent,
      // Além de declará-los!
      ValueComponent
   ],
   imports: [
      BrowserModule,
      // Aqui importando o serviço
      HttpClientModule,
      AppRoutingModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
