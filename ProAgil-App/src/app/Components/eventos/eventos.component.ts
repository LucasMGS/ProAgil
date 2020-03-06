import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from 'src/services/evento.service';
import { Evento } from 'src/app/models/Evento';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

   //PROPS
   eventos: Evento[];
   imagemLargura = 85;
   imagemMargem = 2;
   mostrarImagem = false;
   eventosFiltrados: Evento[];
   modalRef: BsModalRef;
   // tslint:disable-next-line: variable-name
   _filtroLista: string;

  // Construtor
  constructor(private serviceEvento: EventoService, private modalService: BsModalService) { }

  ngOnInit() 
  {
    this.getEventos();
  }
  
  // MÉTODOS
  get filtroLista(): string 
  // tslint:disable-next-line: one-line
  {
     return this._filtroLista;
  }
  set filtroLista(value: string)
  // tslint:disable-next-line: one-line
  {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEvento(this.filtroLista) : this.eventos;
  }

  getEventos() 
  {
       this.serviceEvento.getAllEventos().subscribe(
        (_evento: Evento[]) => 
        {
          this.eventos = _evento;
          this.eventosFiltrados = this.eventos;
          console.log(this.eventosFiltrados);
        },
        error => 
        {
          console.log(error);
        });
  }

  alternarImg() 
  {
     this.mostrarImagem = !this.mostrarImagem;
  }

  filtrarEvento(filtrarPor: string): Evento[]
  {
      filtrarPor = filtrarPor.toLocaleLowerCase();
      return this.eventos.filter(
        evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1);
  }

  openModal(template: TemplateRef<any>) 
  {
     this.modalRef = this.modalService.show(template);
  }
}
