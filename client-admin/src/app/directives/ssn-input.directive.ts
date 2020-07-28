import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appSsnInput]',
})
export class SsnInputDirective {
  constructor(private element: ElementRef) {}

  @HostListener('keyup') OnKeyDown() {
    let currentIndex: number;
    let entryString: string;

    entryString = this.element.nativeElement.value as string;
    currentIndex = entryString.length;
    // debugger;

    if (currentIndex === 4 || currentIndex === 7) {
      this.element.nativeElement.style.type = 'text';
      this.element.nativeElement.style.backgroundColor = 'red';
    } else {
      this.element.nativeElement.style.type = 'password';
      this.element.nativeElement.style.backgroundColor = 'yellow';
    }
  }
}
