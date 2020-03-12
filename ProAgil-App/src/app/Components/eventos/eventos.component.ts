import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from 'src/services/evento.service';
import { Evento } from 'src/app/models/Evento';
import { BsModalService, BsModalRef, defineLocale, ptBrLocale, BsLocaleService, BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { templateJitUrl } from '@angular/compiler';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  
  //PROPS
  eventos: Evento[];
  evento: Evento;
  imagemLargura = 60;
  imagemMargem = 2;
  mostrarImagem = false;
  eventosFiltrados: Evento[];
  modalRef: BsModalRef;
  _filtroLista: string;
  registerForm: FormGroup;
  bodyDeletarEvento = '';
  file: File;
  data = '';
  modoEvento = '';
  fileNameUpdate: string;
  dataAtual: string;
  
  // Construtor
  constructor(private serviceEvento: EventoService, 
  private modalService: BsModalService,
  private formBuilder: FormBuilder,
  private localeService: BsLocaleService,
  private toastr: ToastrService,
  public datepipe: DatePipe) 
  {
    localeService.use('pt-br');
  }
  
  ngOnInit() 
  {
    this.validation();
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
        this.toastr.error(error,'Erro ao tentar carregar eventos');
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
    
  openModal(template: any,evento: Evento = null) 
  {
    this.registerForm.reset();
    if (evento) 
    {
      this.modoEvento = 'put';
      this.evento = Object.assign({}, evento);
      this.fileNameUpdate = evento.imagemURL.toString();
      this.evento.imagemURL = '';
      this.registerForm.patchValue(this.evento);
    }
    else
    {
      this.modoEvento = 'post';
    }
    template.show();
  }
  
  validation()
  {
    this.registerForm = this.formBuilder.group({
      tema: ['',[Validators.required, 
        Validators.minLength(4), 
        Validators.maxLength(50)]],
        local: ['',[Validators.required]],
        data: ['',[]],
        qntdPessoas: ['',[Validators.required, Validators.max(120000)]],
        imagemURL: ['',[Validators.required]],
        telefone: ['',[Validators.required]],
        email: ['',[Validators.required,Validators.email]]
      });
  }
    
    salvarAlteracao(template: any)
    {
      if (this.registerForm.valid) 
      {
        if(this.modoEvento === 'post')
        {
          this.criarEvento(template);
        }      
        else if (this.modoEvento === 'put')
        {
          this.editarEvento(template);      
        }
      }  
    }

    uploadImage()
    {
      if (this.modoEvento === 'post') 
      {
        const nomeDoArquivo = this.evento.imagemURL.split('\\', 3);
        this.evento.imagemURL = nomeDoArquivo[2];
        
        this.serviceEvento.postUpload(this.file, nomeDoArquivo[2]).subscribe(
          () => {
            this.dataAtual = new Date().getMilliseconds().toString();
            this.getEventos();
          }
        );
      }
      else if(this.modoEvento === 'put')
      {
        this.evento.imagemURL = this.fileNameUpdate;
        this.serviceEvento.postUpload(this.file, this.fileNameUpdate).subscribe(
          () => {
            this.dataAtual = new Date().getMilliseconds().toString();
            this.getEventos();
          }
        );
      }
    }

    criarEvento(template: any)
    {
         
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImage();

        this.serviceEvento.postEvento(this.evento).subscribe(
          () =>
          {
            template.hide();
            this.getEventos();
            this.toastr.success('Evento criado com sucesso!')
          },
          error => 
          {
            this.toastr.error(error, 'Erro ao tentar criar evento!')
          });      
    }

    editarEvento(template: any)
    {
      //this.modoEvento = 'put';
      this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);

      this.uploadImage();

      this.serviceEvento.putEvento(this.evento).subscribe
      (
        () =>
        {
          template.hide();
          this.getEventos();
          this.toastr.success('Evento editado com sucesso!')
        },
        error =>
        {
          this.toastr.error(error, 'Erro ao tentar editar!')
        }
      );
    }
      
    registerFormGet(formControlName: string)
    {
        return this.registerForm.get(formControlName);
    }

    excluirEvento(evento: Evento, template: any)
    {
      this.openModal(template);
      this.evento = evento;
      this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
    }
    
    confirmeDelete(template: any) 
    {
      this.serviceEvento.deleteEvento(this.evento.id).subscribe(
        () => {
            template.hide();
            this.getEventos();
            this.toastr.success('Deletado com sucesso!');
          }, 
          error => 
          {
            this.toastr.error(error, 'Erro ao tentar deletar!');
          }
      );
    }

    onFileChange(event)
    {
      if (event.target.files && event.target.files.length) 
      {
        this.file = event.target.files;
        console.log(this.file);  
      }
    }
  }
      