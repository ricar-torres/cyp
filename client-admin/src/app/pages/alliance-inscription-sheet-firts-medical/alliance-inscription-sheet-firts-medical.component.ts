import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import * as jspdf from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-alliance-inscription-sheet-firts-medical',
  templateUrl: './alliance-inscription-sheet-firts-medical.component.html',
  styleUrls: ['./alliance-inscription-sheet-firts-medical.component.css'],
})
export class AllianceInscriptionSheetFirtsMedicalComponent implements OnInit {
  alliance: any;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<
      AllianceInscriptionSheetFirtsMedicalComponent
    >
  ) {
    this.alliance = data.alliance;
  }

  ngOnInit(): void {}

  close() {
    this.dialogRef.close();
  }

  convertToPdf() {
    var data = document.getElementById('pdfContent');
    html2canvas(data).then((canvas) => {
      var imgWidth = 208;
      var pageHeight = 295;
      var imgHeight = (canvas.height * imgWidth) / canvas.width;
      var heightLeft = imgHeight;
      const contentDataURL = canvas.toDataURL('image/jpeg');
      let pdf = new jspdf.default('p', 'mm', 'a4');
      var position = 0;

      pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight);
      heightLeft -= pageHeight;

      while (heightLeft >= 0) {
        position = heightLeft - imgHeight;
        pdf.addPage();
        pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
      }
      pdf.save('newPDF.pdf');
    });
  }
  getAge(dob: string) {
    var diff_ms = Date.now() - Date.parse(dob.substring(0, 10));
    var age_dt = new Date(diff_ms);
    return Math.abs(age_dt.getUTCFullYear() - 1970);
  }
}
