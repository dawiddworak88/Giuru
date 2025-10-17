import React, { useState } from "react";
import PropTypes from "prop-types";
import { 
    Accordion, AccordionDetails, AccordionSummary, Box, Button, Checkbox, 
    Chip, Drawer, IconButton, ListItemText, MenuItem, Select, Stack, Typography, 
    useMediaQuery, useTheme
} from "@mui/material";
import { Close } from "@mui/icons-material";
import { 
    ArrowIcon, CheckboxCheckedIcon, CheckboxIcon,
    FiltersIcon, RemoveIcon 
} from "../../icons";
import NestedFilterAccordionDropdown from "./NestedFilterAccordionDropdown";

function FilterCollector(props) {
    const [sidebarOpen, setSidebarOpen] = useState(false)

    const handleOnSelectFiltersChange = (event) => {
        const {
            target: { value }
        } = event

        const newItem = value[value.length - 1]

        updateFilters(newItem.key, newItem.value, newItem.label)
    }

    const handleOnSidebarFilterSet = (key, value, label) => updateFilters(key, value, label)

    const updateFilters = (key, value, label) => {
        if (isFilterSelected(key, value)) {
            props.setFilters(props.filters.filter(x => !(x.value === value && x.key === key)))
        } else {
            props.setFilters(props.filters.concat({ key, value, label}))
        }
    }
    
    const isFilterSelected = (key, value) => {
        return props.filters.find(x => x.key === key && x.value === value) ? true : false
    }
    
    const handleOnDeleteFilter = (indexToRemove) => {
        props.setFilters(props.filters.filter((_, index) => index !== indexToRemove))
    }

    const handleOnClickClearFilters = () => {
        props.setFilters([])
    }

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const isNotMobile = !isMobile;

    return (
        <div>
            <Box 
                component="div"
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    width: '100%',
                    mb: '1.5rem'
                }}
            >
                {props.total !== 0 && (
                    <Box
                        component="div"
                        sx={{
                            marginRight: "1.5rem"
                        }}
                    >
                        <Typography
                            sx={{
                                fontSize: "0.875rem",
                                fontWeight: 400,
                                lineHeight: "1.5rem",
                                color: "#7C8693"
                            }}
                        >
                            {props.total} {props.resultsLabel}
                        </Typography>
                    </Box>
                )}
                {isNotMobile &&
                    <Box
                        component="div"
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: "1.5rem"
                        }}
                    >
                        {props.filterInputs && props.filterInputs.length > 0 && (
                            <Box
                                sx={{
                                    display: "flex",
                                    gap: "0.75rem",
                                    marginRight: "0.75rem"
                                }}
                            >
                                {props.filterInputs.map((item, index) => (
                                    item.isNested ? (
                                        <NestedFilterAccordionDropdown 
                                            label={item.label}
                                            filterGroups={item.items}
                                            selectedFilters={props.filters}
                                            onChange={updateFilters}
                                            asDropdown={true}
                                        />
                                    ) : (
                                        <Select
                                            key={index}
                                            multiple
                                            displayEmpty
                                            value={props.filters}
                                            sx={{
                                                height: "2.5rem",
                                                borderRadius: "0.25rem",
                                                backgroundColor: "#F7F7F7",
                                                paddingX: "1rem",
                                                paddingY: "0.25rem",
                                                "&& .MuiSelect-select": {
                                                    minHeight: "unset"
                                                }
                                            }}
                                            MenuProps={{
                                                autoFocus: false,
                                                PaperProps: {
                                                    sx: {
                                                        borderRadius: "0.5rem",
                                                        paddingX: "2.5rem",
                                                        paddingY: "2rem"
                                                    }
                                                }
                                            }}
                                            onChange={handleOnSelectFiltersChange}
                                            IconComponent={(props) => <ArrowIcon {...props}/>}
                                            renderValue={() => {
                                                return (
                                                    <Typography 
                                                        sx={{
                                                            display: "flex",
                                                            alignItems: "center",
                                                            fontWeight: 700,
                                                            fontSize: "0.75rem",
                                                            color: "#171717"
                                                        }}
                                                    >
                                                        {item.label}
                                                    </Typography>
                                                );
                                            }}
                                        >
                                            {item.items.map((variant, index) => (
                                                <MenuItem
                                                    key={index}
                                                    value={{
                                                        key: item.key, 
                                                        value: variant.value, 
                                                        label: variant.label 
                                                    }}
                                                    sx={{
                                                        height: "1.5rem",
                                                        padding: 0,
                                                        "&:not(:first-child)": {
                                                            marginTop: "1.5rem"
                                                        }
                                                    }}
                                                >
                                                    <Checkbox
                                                        checked={isFilterSelected(item.key, variant.value)}
                                                        icon={<CheckboxIcon />}
                                                        checkedIcon={<CheckboxCheckedIcon />} 
                                                        sx={{
                                                            padding: 0,
                                                            height: "1.5rem"
                                                        }}
                                                    />
                                                    <ListItemText primary={variant.label} sx={{
                                                        fontSize: "0.875rem",
                                                        fontWeight: 400,
                                                        color: "#171717"
                                                    }}/>
                                                </MenuItem>
                                            ))}
                                        </Select>
                                    )
                                ))}
                            </Box>
                        )}
                    </Box>
                }
                <Button
                    onClick={() => setSidebarOpen(true)}
                    endIcon={<FiltersIcon />}
                    sx={{
                        height: "2.5rem",
                        paddingX: "1rem",
                        paddingY: "0.25rem",
                        borderRadius: "0.25rem",
                        backgroundColor: "#F7F7F7"
                    }}
                >
                    <Typography
                        sx={{
                            fontWeight: 700,
                            fontSize: "0.75rem",
                            color: "#171717"
                        }}
                    >
                        {props.allFilters}
                    </Typography>
                </Button>
                {props.sortItems && props.sortItems.length > 0 &&
                    <Box 
                        sx={{
                            display: "flex",
                            minWidth: "fit-content",
                            alignItems: "center",
                            marginLeft: "auto",
                            gap: "1.75rem"
                        }}
                    >
                        <Typography
                            sx={{
                                fontSize: "0.75rem",
                                fontWeight: 700,
                                color: "#171717"
                            }}
                        >
                            {props.sortLabel}
                        </Typography>
                        <Select
                            IconComponent={(props) => <ArrowIcon {...props}/>}
                            value={props.sorting}
                            onChange={(e) => props.setSorting(e.target.value)}
                            sx={{
                                "&& .MuiSelect-select": {
                                    minHeight: "unset"
                                }
                            }}
                            MenuProps={{
                                autoFocus: false,
                                PaperProps: {
                                    sx: {
                                        paddingX: "1.5rem",
                                        paddingY: "1rem"
                                    }
                                }
                            }}
                            renderValue={(value) => {
                                return (
                                    <Typography
                                        sx={{
                                            fontSize: "0.75rem",
                                            fontWeight: 700,
                                            color: "#064254"
                                        }}
                                    >
                                        {props.sortItems.find(item => item.key === value)?.label}
                                    </Typography>
                                )
                            }}
                        >
                            {props.sortItems.map((item, index) => (
                                <MenuItem
                                    key={index}
                                    value={item.key}
                                    sx={{
                                        "&:not(:first-child)": {
                                            marginTop: "1rem"
                                        }
                                    }}
                                >
                                    {item.label}    
                                </MenuItem>
                            ))}
                        </Select>
                    </Box>
                }
            </Box>
            {props.filters && props.filters.length > 0 && (
                <Box 
                    sx={{
                        display: 'flex',
                        gap: '0.75rem',
                        marginBottom: "2.5rem"
                    }}
                >
                    {props.filters.map((selectedFilter, index) => (
                        <Chip
                            key={index}
                            label={selectedFilter.label}
                            onDelete={() => handleOnDeleteFilter(selectedFilter)}
                            deleteIcon={<RemoveIcon />} 
                            sx={{
                                paddingY: "0.25rem",
                                paddingX: "1rem",
                                display: "flex",
                                gap: "0.25rem",
                                width: "fit-content",
                                borderRadius: "0.25rem",
                                backgroundColor: "#064254",
                                border: "1px solid #064254",
                                color: "#FFFFFF",
                                fontSize: "0.75rem",
                                fontWeight: 700
                            }}
                        />
                    ))}
                    {props.filters.length > 1 && 
                        <Button
                            onClick={handleOnClickClearFilters}
                            sx={{
                                paddingY: "0.25rem",
                                paddingX: "1rem",
                                borderRadius: "0.25rem",
                                backgroundColor: "#F8F9FC",
                                border: "1px solid #D5D7D8",
                                color: "#171717",
                                fontSize: "0.75rem",
                                fontWeight: 700
                            }}
                        >
                            {props.clearAllFilters}
                        </Button>
                    }
                </Box>   
            )}
            {props.filterInputs && props.filterInputs.length > 0 && (
                <Drawer
                    anchor="right"
                    open={sidebarOpen}
                    onClose={() => setSidebarOpen(false)}
                >
                    <Box 
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            minWidth: "28.875rem",
                            minHeight: "100vh"
                        }}
                    >
                        <Box 
                            sx={{
                                display: "flex",
                                justifyContent: "space-between",
                                alignItems: "center",
                                padding: "0 1.875rem 0 1.5rem",
                                minHeight: "5rem"
                            }}
                        >
                            <Typography 
                                sx={{
                                    fontSize: "1.25rem",
                                    fontWeight: 700,
                                    color: "#171717"
                                }}    
                            >
                                {props.filtersLabel}
                            </Typography>
                            <IconButton
                                disableRipple
                                disableTouchRipple
                                disableElevation
                                onClick={() => setSidebarOpen(false)}
                                sx={{
                                    color: "#171717"
                                }}
                            >
                                <Close />
                            </IconButton>
                        </Box>
                        <Box 
                            sx={{
                                paddingX: "1.5rem"
                            }}
                        >
                            {props.filterInputs.map((item, index) => (
                                item.isNested ? (
                                    <NestedFilterAccordionDropdown 
                                        key={index}
                                        label={item.label}
                                        filterGroups={item.items}
                                        selectedFilters={props.filters}
                                        onChange={updateFilters}
                                    />
                                ) : (
                                    <Accordion 
                                        key={index}
                                        elevation={0}
                                        disableGutters
                                        square
                                        sx={{
                                            borderTop: "1px solid #D5D7D8",
                                            // borderBottom: "1px solid #D5D7D8",
                                            "&&:before": {
                                                display: "none"
                                            }
                                        }}
                                    >
                                        <AccordionSummary
                                            expandIcon={<ArrowIcon />}
                                            sx={{
                                                paddingY: "1rem",
                                                fontWeight: 700,
                                                fontSize: "0.875rem",
                                                color: "#171717",
                                                lineHeight: "1.5rem",
                                                letterSpacing: "0.14px"
                                            }}
                                        >
                                            {item.label}
                                        </AccordionSummary>
                                        <AccordionDetails key={index} className="sidebar__filters__filter__item is-flex is-align-items-center">
                                            <Stack
                                                spacing="1.5rem"
                                            >
                                                {item.items.map((variant, index) => (
                                                    <Stack direction="row">
                                                        <Checkbox
                                                        className="sidebar__filters__filter__item__checkbox"
                                                        checked={isFilterSelected(item.key, variant.value)}
                                                        icon={<CheckboxIcon />}
                                                        checkedIcon={<CheckboxCheckedIcon />}
                                                        onClick={() => handleOnSidebarFilterSet(item.key, variant.value, variant.label) } />
                                                        <Typography>{variant.label}</Typography>
                                                    </Stack>
                                                ))}    
                                            </Stack>        
                                        </AccordionDetails>
                                </Accordion>
                            )))}
                        </Box>
                        <Box 
                            sx={{
                                padding: "1.5rem",
                                marginTop: "auto",
                                display: "flex",
                                gap: "0.5rem"
                            }}
                        >
                            <Button
                                className="sidebar__fotter__button button-clear py-3"
                                onClick={handleOnClickClearFilters}
                                disabled={!props.filters.length > 0}
                            >
                                <Typography fontWeight={700}>
                                    {props.clearAllFilters}
                                </Typography>
                            </Button>
                            <Button
                                className="sidebar__fotter__button py-3"
                                onClick={() => setSidebarOpen(false)}
                            >
                                <Typography fontWeight={700}>
                                    {props.seeResult}
                                </Typography>
                            </Button>
                        </Box>
                    </Box>
                </Drawer>
            )}
        </div>
    )
}

FilterCollector.propTypes = {
    sortLabel: PropTypes.string,
    sortItems: PropTypes.array,
    total: PropTypes.number,
    resultsLabel: PropTypes.string,
    filterInputs: PropTypes.array,
    allFiltersLabel: PropTypes.string,
    clearAllFiltersLabel: PropTypes.string,
    filtersLabel: PropTypes.string,
    seeResultLabel: PropTypes.string
}

export default FilterCollector;