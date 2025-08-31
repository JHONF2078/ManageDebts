import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DebtService } from '../../services/debt.service';
import { MATERIAL_IMPORTS } from '../../../material/material.component';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-debt-pay',
    standalone: true,
    imports: [CommonModule, ...MATERIAL_IMPORTS],
    templateUrl: './debt-pay.component.html',
    styleUrl: './debt-pay.component.scss'
})
export class DebtPayComponent {
    constructor(
        public dialogRef: MatDialogRef<DebtPayComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private debtService: DebtService
    ) { }

    pay(): void {
        this.debtService.payDebt(this.data.id).subscribe({
            next: (result) => this.dialogRef.close(result),
            error: () => alert('Error al pagar la deuda')
        });
    }

    onCancel(): void {
        this.dialogRef.close();
    }
}
