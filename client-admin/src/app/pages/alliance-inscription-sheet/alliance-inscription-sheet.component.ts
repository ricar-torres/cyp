import { Component, OnInit } from '@angular/core';
import * as jspdf from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-alliance-inscription-sheet',
  templateUrl: './alliance-inscription-sheet.component.html',
  styleUrls: ['./alliance-inscription-sheet.component.css'],
})
export class AllianceInscriptionSheetComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}

  convertToPdf() {
    var data = document.getElementById('pdfContent');
    html2canvas(data).then((canvas) => {
      var imgWidth = 208;
      var imgHeight = (canvas.height * imgWidth) / canvas.width;
      const contentDataURL = canvas.toDataURL('image/jpeg');
      let pdf = new jspdf.default('p', 'mm', 'a4');
      var position = 0;
      pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight);
      pdf.save('newPDF.pdf');
    });
  }
}
