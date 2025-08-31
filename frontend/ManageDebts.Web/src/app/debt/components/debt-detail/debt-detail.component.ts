import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { DebtService } from '../../services/debt.service';
import { Debt } from '../../interface/debt.model';

@Component({
  selector: 'app-debt-detail',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './debt-detail.component.html',
  styleUrls: ['./debt-detail.component.scss']
})
export class DebtDetailComponent {
  debt?: Debt;
  loading = true;
  error: string | null = null;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { id: string },
    private dialogRef: MatDialogRef<DebtDetailComponent>,
    private debtService: DebtService
  ) {
    this.fetchDebtDetail();
  }

  fetchDebtDetail() {
    this.debtService.getDebtById(this.data.id).subscribe({
      next: (debt: Debt) => {
        this.debt = debt;
        this.loading = false;
      },
      error: () => {
        this.error = 'No se pudo cargar el detalle de la deuda.';
        this.loading = false;
      }
    });
  }

  close() {
    this.dialogRef.close();
  }
}
