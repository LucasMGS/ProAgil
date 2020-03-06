import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
import { DateTimeParse } from '../utils/DateTimeParse';

@Pipe({
  name: 'DateTimeFormatPipe'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  transform(value: any, args?: any): any 
  {
     return super.transform(value, DateTimeParse.DATE_TIME_FMT);
  }

}
