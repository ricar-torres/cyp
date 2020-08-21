import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'PreviewSsn' })
export class PreviewSsn implements PipeTransform {
  transform(value: string): string {
    let ssn = 'xxx-xx-' + value.substring(5);
    return ssn;
  }
}
