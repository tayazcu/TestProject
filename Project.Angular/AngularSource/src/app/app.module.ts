import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { configurationFactory } from './services/initializer/initializer.factory';
import { UrlConfigService } from './services/url-config.service';
import { HttpClientModule } from '@angular/common/http';
import { FirstPageComponent } from './first-page/first-page.component';

@NgModule({
  declarations: [
    AppComponent,
    FirstPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: configurationFactory,
      multi: true,
      deps: [
        UrlConfigService,
      ],
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
