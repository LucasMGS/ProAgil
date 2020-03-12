// MODULE IMPORTS
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BsDropdownModule, TooltipModule, ModalModule } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

// SERVICE IMPORTS
import { EventoService } from 'src/services/evento.service';

// COMPONENT IMPORTS
import { AppComponent } from './app.component';
import { EventosComponent } from './Components/eventos/eventos.component';
import { NavComponent } from './Components/nav/nav.component';
import { ContatosComponent } from './Components/contatos/contatos.component';
import { PalestrantesComponent } from './Components/palestrantes/palestrantes.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';

// PIPE IMPORTS
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { TituloComponent } from './Components/_shared/titulo/titulo.component';
import { DatePipe } from '@angular/common';

@NgModule({
   declarations: [
      AppComponent,
      EventosComponent,
      NavComponent,
      DateTimeFormatPipe,
      ContatosComponent,
      PalestrantesComponent,
      DashboardComponent,
      TituloComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      TooltipModule.forRoot(),
      ModalModule.forRoot(),
      BrowserAnimationsModule,
      ReactiveFormsModule,
      BsDatepickerModule.forRoot(),
      ToastrModule.forRoot({
         timeOut: 10000,
         positionClass: 'toast-bottom-right',
         preventDuplicates: true,
       })
   ],
   providers: [
      EventoService,
      DatePipe
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
