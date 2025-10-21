import React from "react"
import { Box, Button } from "@mui/material"
import PropTypes from "prop-types";
import { 
    Splide, SplideSlide 
} from "@splidejs/react-splide";

const OrdersStatusFilters = ({
    ordersStatuses = [],
    selectedStatusId,
    onStatusChange,
    defaultLabel
}) => {
    return (
        ordersStatuses.length > 0 && (
            <Box
                component="div"
                sx={{
                    marginBottom: "2rem",
                }}
            >
                <Splide
                    options={{
                        autoWidth: true,
                        arrows: false,
                        pagination: false,
                        gap: "0.5rem",
                        height: "3.6875rem"
                    }}
                >
                    <SplideSlide>
                        <Button
                            disableRipple
                            onClick={() => onStatusChange(null)}
                            sx={{
                                backgroundColor: "whiteBase",
                                borderRadius: "0.25rem",
                                border: 1,
                                borderStyle: "solid",
                                borderColor: "gray.300",
                                padding: "1rem",
                                fontSize: "0.875rem",
                                fontWeight: 400,
                                color: "blackBase",
                                ...(selectedStatusId === null || !selectedStatusId) && {
                                    backgroundColor: "mint.500",
                                    borderColor: "mint.500",
                                    color: "whiteBase"
                                },
                                "&&:hover": {
                                    color: "mint.500",
                                    borderColor: "mint.300"
                                }
                            }}
                        >
                            {defaultLabel}
                        </Button>
                    </SplideSlide>
                    {ordersStatuses.map((orderStatus, index) => (
                        <SplideSlide key={index}>
                            <Button
                                key={index}
                                disableRipple
                                onClick={() => onStatusChange(orderStatus.id)}
                                sx={{
                                    backgroundColor: "whiteBase",
                                    borderRadius: "0.25rem",
                                    border: 1,
                                    borderStyle: "solid",
                                    borderColor: "gray.300",
                                    padding: "1rem",
                                    fontSize: "0.875rem",
                                    fontWeight: 400,
                                    color: "blackBase",
                                    ...(selectedStatusId === orderStatus.id) && {
                                        backgroundColor: "mint.500",
                                        borderColor: "mint.500",
                                        color: "whiteBase"
                                    },
                                    "&&:hover": {
                                        color: "mint.500",
                                        borderColor: "mint.300"
                                    }
                                }}
                            >{orderStatus.name}</Button>
                        </SplideSlide>
                    ))}
                </Splide>
            </Box>
        )
    )
}

OrdersStatusFilters.propTypes = {
    ordersStatuses: PropTypes.arrayOf(PropTypes.shape({
        id: PropTypes.string.isRequired,
        name: PropTypes.string.isRequired
    })).isRequired,
    selectedStatusId: PropTypes.oneOfType([PropTypes.string, PropTypes.oneOf([null])]),
    onStatusChange: PropTypes.func.isRequired,
    defaultLabel: PropTypes.string.isRequired
}

export default OrdersStatusFilters;