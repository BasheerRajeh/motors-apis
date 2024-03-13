import { Row, Col, Card } from "react-bootstrap";
// components
import TableV8, { defaultPagination } from "../../components/table-v8";
import { useEffect, useState } from "react";
import { ColumnDef, PaginationState } from "@tanstack/react-table";
import useFetch from "../../hooks/use-fetch";

export default function ViewBookings() {
  const [bookings, setBookings] = useState<any>([]);
  const [pagination, setPagination] =
    useState<PaginationState>(defaultPagination);
  const { get } = useFetch();
  useEffect(
    function () {
      (async function () {
        const temp = await get("/bookings", {
          query: { PageSize: pagination.pageSize, Page: pagination.pageIndex },
        });
        setBookings(temp);
      })();
    },
    [pagination.pageSize, pagination.pageIndex]
  );
  return (
    <TableV8
      columns={columns}
      data={bookings.Data || []}
      pageCount={bookings.PageCount || 0}
      {...{ pagination, setPagination }}
    />
  );
}

const columns: ColumnDef<any>[] = [
  {
    header: "ID",
    accessorKey: "Id",
  },
  {
    header: "Full Name",
    accessorKey: "FullName",
  },
  {
    header: "Contact",
    accessorKey: "PhoneNumber",
  },
  {
    header: "Email",
    accessorKey: "Email",
  },
  {
    header: "Date",
    accessorKey: "Date",
  },
];
