import { BootstrapModule } from './modules/bootstrap/bootstrap.module';
import { HttpInterceptorService } from './service/http-interceptor.service';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { LoginComponent } from './pages/login-page/login.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastComponent } from './components/toast/toast.component';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { NavbarComponent } from './components/navbar/navbar.component';
import { GamePageComponent } from './pages/game-page/game-page.component';
import { ChatroomComponent } from './components/chatroom/chatroom.component';
import { SurveyPageComponent } from './pages/survey-page/survey-page.component';
import { LoadingComponent } from './components/loading/loading.component';
import { HamsterWheelComponent } from './components/hamster-wheel/hamster-wheel.component';
import { CardComponent } from './components/card/card.component';
import { SamplePageComponent } from './pages/sample-page/sample-page.component';



@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    LoginComponent,
    ToastComponent,
    SafeHtmlPipe,
    NavbarComponent,
    GamePageComponent,
    ChatroomComponent,
    SurveyPageComponent,
    LoadingComponent,
    HamsterWheelComponent,
    CardComponent,
    SamplePageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BootstrapModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorService,
      multi: true
    },
  ],
  bootstrap: [AppComponent],
  exports: [
    LoadingComponent
  ]
})
export class AppModule { }
