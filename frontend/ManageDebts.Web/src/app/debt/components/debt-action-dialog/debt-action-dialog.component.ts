import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface DebtActionDialogData {
  title: string;
  message: string;
  confirmText: string;
  confirmColor: 'primary' | 'warn';
}

import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-debt-action-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './debt-action-dialog.component.html',
})
export class DebtActionDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DebtActionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DebtActionDialogData
  ) { }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
