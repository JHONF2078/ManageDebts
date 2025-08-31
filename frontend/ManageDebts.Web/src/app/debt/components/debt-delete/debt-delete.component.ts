import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DebtService } from '../../services/debt.service';
import { MATERIAL_IMPORTS } from '../../../material/material.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-debt-delete',
  standalone: true,
  imports: [CommonModule, ...MATERIAL_IMPORTS],
  templateUrl: './debt-delete.component.html',
  styleUrl: './debt-delete.component.scss'
})
export class DebtDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<DebtDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private debtService: DebtService
  ) { }

  delete(): void {
    this.debtService.deleteDebt(this.data.id).subscribe({
      next: (result) => this.dialogRef.close(result),
      error: (err) => {
        const msg = err?.error || 'Error al eliminar la deuda';
        alert(msg);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
