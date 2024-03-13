import React, {
  Dispatch,
  Fragment,
  PropsWithChildren,
  SetStateAction,
  useEffect,
} from "react";

import {
  useReactTable,
  getCoreRowModel,
  getExpandedRowModel,
  ColumnDef,
  getSortedRowModel,
  SortingState,
  flexRender,
  Row,
  PaginationState,
} from "@tanstack/react-table";

type TableProps<TData> = {
  data: TData[];
  pageCount: number;
  columns: ColumnDef<TData>[];
  renderSubComponent: (props: { row: Row<TData> }) => React.ReactElement;
  getRowCanExpand: (row: Row<TData>) => boolean;
  pagination?: PaginationState;
  setPagination?: Dispatch<SetStateAction<PaginationState>>;
};

function Table({
  data,
  columns,
  renderSubComponent,
  getRowCanExpand,
  pageCount,
  pagination = { pageIndex: 0, pageSize: 2 },
  setPagination,
}: TableProps<any>): JSX.Element {
  const [sorting, setSorting] = React.useState<SortingState>([]);
  const table = useReactTable<any>({
    data,
    pageCount: pageCount,
    state: {
      sorting,
      pagination,
    },
    columns,
    getRowCanExpand,

    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getExpandedRowModel: getExpandedRowModel(),
    getSortedRowModel: getSortedRowModel(),
    manualPagination: !!setPagination,
    // getPaginationRowModel: getPaginationRowModel(),
  });
  useEffect(() => {
    table.setPageCount(pageCount);
  }, [pageCount]);
  return (
    <div className="p-2">
      <div className="h-2" />
      <select
        value={table.getState().pagination.pageSize}
        onChange={(e) => {
          table.setPageSize(Number(e.target.value));
        }}
      >
        {[10, 20, 50, 100].map((pageSize) => (
          <option key={pageSize} value={pageSize}>
            {pageSize}
          </option>
        ))}
      </select>
      <table className="table table-centered react-table">
        <thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => {
                return (
                  <th key={header.id} colSpan={header.colSpan}>
                    {header.isPlaceholder ? null : (
                      <div
                        {...{
                          className: header.column.getCanSort()
                            ? "cursor-pointer select-none"
                            : "",
                          onClick: header.column.getToggleSortingHandler(),
                        }}
                      >
                        {flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                        {{
                          asc: " 🔼",
                          desc: " 🔽",
                        }[header.column.getIsSorted() as string] ?? null}
                      </div>
                    )}
                  </th>
                );
              })}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.map((row) => {
            return (
              <Fragment key={row.id}>
                <tr>
                  {/* first row is a normal row */}
                  {row.getVisibleCells().map((cell) => {
                    return (
                      <td key={cell.id}>
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </td>
                    );
                  })}
                </tr>
                {row.getIsExpanded() && (
                  <tr>
                    {/* 2nd row is a custom 1 cell row */}
                    <td colSpan={row.getVisibleCells().length}>
                      {renderSubComponent({ row })}
                    </td>
                  </tr>
                )}
              </Fragment>
            );
          })}
        </tbody>
      </table>
      <div className="h-2" />
      <div
        className="flex items-center gap-2"
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <span className="flex items-center gap-1">
          Page
          <strong>
            {table.getState().pagination.pageIndex + 1} of{" "}
            {table.getPageCount()}
          </strong>
        </span>
        <div>
          <button
            className="border rounded p-1"
            onClick={() => table.setPageIndex(0)}
            disabled={!table.getCanPreviousPage()}
          >
            {"<<"}
          </button>
          <button
            className="border rounded p-1"
            onClick={() => table.previousPage()}
            disabled={!table.getCanPreviousPage()}
          >
            {"<"}
          </button>
          <button
            className="border rounded p-1"
            onClick={() => table.nextPage()}
            disabled={!table.getCanNextPage()}
          >
            {">"}
          </button>
          <button
            className="border rounded p-1"
            onClick={() => table.setPageIndex(table.getPageCount() - 1)}
            disabled={!table.getCanNextPage()}
          >
            {">>"}
          </button>

          <span className="flex items-center gap-1">
            {" "}
            | Go to page:
            <input
              min={1}
              max={pageCount}
              type="number"
              defaultValue={table.getState().pagination.pageIndex + 1}
              onChange={(e) => {
                const page = e.target.value ? Number(e.target.value) - 1 : 0;
                table.setPageIndex(page);
              }}
              className="border p-1 rounded w-16"
            />
          </span>
        </div>
      </div>
    </div>
  );
}

const renderSubComponent = ({ row }: { row: Row<any> }) => {
  return (
    <pre style={{ fontSize: "10px" }}>
      <code>{JSON.stringify(row.original, null, 2)}</code>
    </pre>
  );
};

export default function TableV8<T>(
  props: PropsWithChildren<TablePropsType<T>>
) {
  if (props.error) {
    console.log(props.error);
  }
  return (
    <Table
      pagination={props.pagination}
      setPagination={props.setPagination}
      data={props.data}
      pageCount={props.pageCount}
      columns={props.columns}
      getRowCanExpand={() => true}
      renderSubComponent={renderSubComponent}
    />
  );
}
type TablePropsType<T> = {
  columns: ColumnDef<T>[];
  data: T[];
  pageCount: number;
  error?: string;
  loading?: boolean;
  pagination?: PaginationState;
  setPagination?: Dispatch<SetStateAction<PaginationState>>;
};

export const defaultPagination: PaginationState = {
  pageIndex: 0,
  pageSize: 10,
};
