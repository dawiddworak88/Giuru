import { Box, Button } from "@mui/material"

const OrdersStatusFilters = ({
    ordersStatuses = [],
    selectedStatusId,
    onStatusChange
}) => {
    return (
        ordersStatuses.length > 0 && (
            <Box
                component="div"
                sx={{
                    display: "flex",
                    gap: "0.5rem",
                    marginBottom: "2rem"
                }}
            >
                <Button
                    disableRipple
                    onClick={() => onStatusChange(null)}
                    sx={{
                        backgroundColor: "#FFF",
                        borderRadius: "0.25rem",
                        border: "1px solid #D5D7D8",
                        padding: "1rem",
                        fontSize: "0.875rem",
                        fontWeight: 400,
                        color: "#171717",
                        ...(selectedStatusId === null || !selectedStatusId) && {
                            backgroundColor: "#064254",
                            borderColor: "#064254",
                            color: "#FFF"
                        },
                        "&&:hover": {
                            color: "#064254",
                            borderColor: "#386876"
                        }
                    }}
                >
                    All
                </Button>
                {ordersStatuses.map((orderStatus, index) => (
                    <Button
                        key={index}
                        disableRipple
                        onClick={() => onStatusChange(orderStatus.id)}
                        sx={{
                            backgroundColor: "#FFF",
                            borderRadius: "0.25rem",
                            border: "1px solid #D5D7D8",
                            padding: "1rem",
                            fontSize: "0.875rem",
                            fontWeight: 400,
                            color: "#171717",
                            ...(selectedStatusId === orderStatus.id) && {
                                backgroundColor: "#064254",
                                borderColor: "#064254",
                                color: "#FFF"
                            },
                            "&&:hover": {
                                color: "#064254",
                                borderColor: "#064254"
                            }
                        }}
                    >{orderStatus.name}</Button>
                ))}
            </Box>
        )
    )
}

export default OrdersStatusFilters;