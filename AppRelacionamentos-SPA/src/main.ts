import { enableProdMode } from '@angular/core';
// Ele diz que nós estamos fazendo uma WEB APP que roda no browser
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

// Esse comando diz pra usar o bootstrap no AppModule que irá rodar com o HTML da página.
platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));