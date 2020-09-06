import { Component, OnInit, Inject } from '@angular/core';
import * as jspdf from 'jspdf';
import html2canvas from 'html2canvas';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-alliance-inscription-sheet',
  templateUrl: './alliance-inscription-sheet.component.html',
  styleUrls: ['./alliance-inscription-sheet.component.css'],
})
export class AllianceInscriptionSheetComponent implements OnInit {
  alliance: any;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AllianceInscriptionSheetComponent>
  ) {
    this.alliance = data.alliance;
    console.log(this.alliance);
  }

  ngOnInit(): void {}

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

  getDependantTypeOne(dependant: []) {
    if (dependant) {
      var res = dependant.find((x) => (<any>x).relationship == 1);
      if (res) return res;
    }
    return undefined;
  }

  getRelation(dependent) {
    var relation = '';
    switch (dependent.relationship) {
      case 1:
        relation = 'Cónyuge';
        break;
      case 2:
        relation = 'Hijo/a';
        break;
      case 3:
        relation = 'Hijastro/a';
        break;
      case 4:
        relation = 'Adoptivo/a';
        break;
      default:
        break;
    }
    return relation;
  }

  addonInclueded(addon) {
    var isInList = (<[]>this.alliance.addonList).findIndex(
      (x) => x == addon.id
    );
    if (isInList > -1) return true;
    return false;
  }
}
