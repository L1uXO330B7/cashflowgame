import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { SharedMaterialModule } from './common/modules/shared/shared-material.module';
import { UsersTableComponent } from './pages/users-table/users-table.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpInterceptorService } from './common/services/http-interceptor.service';


@NgModule({
  declarations: [
    AppComponent,
    DashboardPageComponent,
    UsersTableComponent,
    LoginPageComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedMaterialModule,
    HttpClientModule

  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
