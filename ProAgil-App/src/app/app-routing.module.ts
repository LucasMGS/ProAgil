import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EventosComponent } from './Components/eventos/eventos.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { PalestrantesComponent } from './Components/palestrantes/palestrantes.component';
import { ContatosComponent } from './Components/contatos/contatos.component';


const routes: Routes = [
   { path: 'eventos', component: EventosComponent },
   { path: 'dashboard', component: DashboardComponent },
   { path: 'palestrantes', component: PalestrantesComponent },
   { path: 'contatos', component: ContatosComponent },
   { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
   { path: '**', redirectTo: 'dashboard', pathMatch: 'full'}
];

@NgModule({
   imports: [
      RouterModule.forRoot(routes)
   ],
   exports: [
      RouterModule
   ],
   declarations: [
   ]
})
export class AppRoutingModule { }
