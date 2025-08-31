import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MATERIAL_IMPORTS } from '../../../material/material.component';

@Component({
    selector: 'app-debt-filter',
    standalone: true,
    imports: [CommonModule, ...MATERIAL_IMPORTS],
    templateUrl: './debt-filter.component.html',
    styleUrl: './debt-filter.component.scss'
})
export class DebtFilterComponent {
    @Output() filterChange = new EventEmitter<{ pay?: string, debtor?: string, creditor?: string }>();

    pay: string = '';
    debtor: string = '';
    creditor: string = '';

    onFilterChange() {
        this.filterChange.emit({ pay: this.pay, debtor: this.debtor, creditor: this.creditor });
    }
}
