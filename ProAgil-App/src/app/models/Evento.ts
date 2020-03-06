import { Lote } from './Lote';
import { Palestrante } from './Palestrante';
import { RedeSocial } from './RedeSocial';

export interface Evento 
{
    id: number;
    local: number;
    data?: Date;
    tema: string;
    qntdPessoas: number;
    imagemURL: string;
    telefone: number;
    email: string;
    lotes: Lote[];
    redesSociais: RedeSocial[];
    palestranteEventos: Palestrante[];
}
