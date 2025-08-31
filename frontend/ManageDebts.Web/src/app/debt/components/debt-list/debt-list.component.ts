import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild, Injectable } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../material/material.component';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { DebtService } from '../../services/debt.service';
import { Debt } from '../../interface/debt.model';
import { DebtCreateComponent } from '../debt-create/debt-create.component';
import { DebtEditComponent } from '../debt-edit/debt-edit.component';
import { DebtPayComponent } from '../debt-pay/debt-pay.component';
import { DebtDeleteComponent } from '../debt-delete/debt-delete.component';
import { DebtDetailComponent } from '../debt-detail/debt-detail.component';
import { DebtFilterComponent } from '../debt-filter/debt-filter.component';
import { Router } from '@angular/router';

@Injectable()
export class MyCustomPaginatorIntl extends MatPaginatorIntl {
  override itemsPerPageLabel = 'Rows per page';
}

@Component({
  selector: 'app-debt-list',
  standalone: true,
  imports: [CommonModule, ...MATERIAL_IMPORTS, DebtFilterComponent],
  templateUrl: './debt-list.component.html',
  styleUrl: './debt-list.component.scss',
  providers: [
    { provide: MatPaginatorIntl, useClass: MyCustomPaginatorIntl }
  ]
})
export class DebtListComponent implements OnInit, OnDestroy, AfterViewInit {
  openDebtDetail(debtId: string): void {
    this.dialog.open(DebtDetailComponent, {
      data: { id: debtId },
      width: '400px',
      autoFocus: true,
      restoreFocus: true
    });
  }
  private subscriptions: Subscription = new Subscription();


  public dataSource = new MatTableDataSource<Debt>();

  @ViewChild(MatSort) order!: MatSort;
  @ViewChild(MatPaginator) pagination!: MatPaginator;

  displayedColumns: string[] = ['creditorName', 'debtorName', 'amount', 'description', 'isPaid', 'createdAt', 'paidAt', 'actions'];

  originalDebts: Debt[] = [];

  constructor(private debtService: DebtService,
    private router: Router, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadDebts();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  applyFilter(value: string): void {
    if (value.trim() === '') {
      this.loadDebts(); // Si el campo está vacío, recarga todos los datos
      return;
    }

    const searchValue = value.trim();
    const searchSubscription = this.debtService.searchDebts(searchValue)
      .subscribe(filteredDebts => {
        this.dataSource.data = filteredDebts;
      });
    this.subscriptions.add(searchSubscription);
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.order;
    this.dataSource.paginator = this.pagination;
  }

  loadDebts(): void {
    const DebtSubscription = this.debtService.getDebts().subscribe(debts => {
      this.dataSource.data = debts;
      this.originalDebts = debts;
    });
    this.subscriptions.add(DebtSubscription);
  }

  onFilterChange(filter: { pay?: string, debtor?: string, creditor?: string }) {
    let filtered = [...this.originalDebts];
    if (filter.pay === 'true') {
      filtered = filtered.filter(d => d.isPaid);
    } else if (filter.pay === 'false') {
      filtered = filtered.filter(d => !d.isPaid);
    }
    if (filter.debtor) {
      filtered = filtered.filter(d => d.debtorName?.toLowerCase().includes(filter.debtor ? filter.debtor.toLowerCase() : ""));
    }
    if (filter.creditor) {
      filtered = filtered.filter(d => d.creditorName?.toLowerCase().includes(filter.creditor ? filter.creditor.toLowerCase() : ""));
    }
    this.dataSource.data = filtered;
  }

  openDialogNew(debts?: Debt, order?: Debt): void {
    const dialogRef = this.dialog.open(DebtCreateComponent, {
      width: '700px',
      data: {
        order: order || {},
        listDebts: debts || {}
      },
      autoFocus: true,
      restoreFocus: true
    });

    const dialogSubscription = dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.loadDebts();
      }
    });

    this.subscriptions.add(dialogSubscription);
  }


  openDialogEdit(debt: Debt): void {
    const dialogRef = this.dialog.open(DebtEditComponent, {
      width: '700px',
      data: debt,
      autoFocus: true,
      restoreFocus: true
    });
    const dialogSubscription = dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.loadDebts();
      }
    });
    this.subscriptions.add(dialogSubscription);
  }

  openDialogPay(debt: Debt): void {
    const dialogRef = this.dialog.open(DebtPayComponent, {
      width: '400px',
      data: debt,
      autoFocus: true,
      restoreFocus: true
    });
    const dialogSubscription = dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        // Actualizar solo la fila pagada sin recargar toda la lista
        const idx = this.dataSource.data.findIndex(d => d.id === debt.id);
        if (idx > -1) {
          this.dataSource.data[idx] = { ...this.dataSource.data[idx], isPaid: true, paidAt: result.paidAt };
          // Forzar actualización de la tabla
          this.dataSource._updateChangeSubscription();
        }
      }
    });
    this.subscriptions.add(dialogSubscription);
  }

  openDialogDelete(debt: Debt): void {
    const dialogRef = this.dialog.open(DebtDeleteComponent, {
      width: '400px',
      data: debt,
      autoFocus: true,
      restoreFocus: true
    });
    const dialogSubscription = dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.loadDebts();
      }
    });
    this.subscriptions.add(dialogSubscription);
  }

}
