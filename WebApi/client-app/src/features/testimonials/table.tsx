import TableV8, { defaultPagination } from "../../components/table-v8";
import { useEffect, useRef, useState } from "react";
import { ColumnDef, PaginationState } from "@tanstack/react-table";
import AppDropdown from "../../components/dropdown";
import { useNavigate } from "react-router-dom";
import useFetch from "../../hooks/use-fetch";
import { toLocalDateTime } from "../../utils/dates";

export default function ViewTestimonials() {
  const [testimonials, setTestimonials] = useState<any>({});
  const [pagination, setPagination] =
    useState<PaginationState>(defaultPagination);
  const { get } = useFetch();
  useEffect(() => {
    (async function () {
      var temp = await get("/testimonials", {
        query: { PageSize: pagination.pageSize, Page: pagination.pageIndex },
      });
      setTestimonials(temp || {});
    })();
  }, [pagination.pageSize, pagination.pageIndex]);

  return (
    <TableV8
      columns={columns}
      data={testimonials.Data || []}
      pageCount={testimonials.PageCount || 0}
      {...{ pagination, setPagination }}
    />
  );
}

function RowActions({ row }: any) {
  const navigate = useNavigate();

  return (
    <AppDropdown
      title=""
      options={[
        {
          label: "Edit",
          onClick: () => navigate(`/testimonials/form/${row.Id}`),
        },
      ]}
    />
  );
}

const columns: ColumnDef<any>[] = [
  {
    header: "ID",
    accessorKey: "Id",
  },
  {
    header: "Name",
    accessorKey: "Name",
  },
  {
    header: "Title",
    accessorKey: "Title",
  },
  {
    header: "CreatedDate",
    accessorKey: "CreatedDate",
    cell: ({ row }) => <>{toLocalDateTime(row.original.CreatedDate)}</>,
  },
  {
    header: "UpdatedDate",
    accessorKey: "UpdatedDate",
    cell: ({ row }) => <>{toLocalDateTime(row.original.UpdatedDate)}</>,
  },
  {
    header: "Action",
    // accessorKey: "Id",
    cell: ({ row }) => <RowActions row={row.original} />,
  },
];
