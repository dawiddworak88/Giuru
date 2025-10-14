import React, { useState } from "react";
import PropTypes from "prop-types";
import { 
    Accordion, AccordionDetails, AccordionSummary, Box, Button, Checkbox, 
    Chip, Drawer, ListItemText, MenuItem, Select, Typography, 
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
            <div className="active-filters">
                {props.filters.map((item, index) => (
                    <Chip
                        className="active-filters__item pr-3 mr-3 mb-3"
                        key={index}
                        label={item.label}
                        onDelete={() => handleOnDeleteFilter(index)}
                        deleteIcon={<RemoveIcon />} />
                ))}
                {props.filters.length > 1 && 
                    <Button
                        className="active-filters__button button-clear px-3 py-1 mb-3 has-text-weight-bold"
                        onClick={handleOnClickClearFilters}
                    >{props.clearAllFilters}</Button>
                }
            </div>
            <Drawer
                anchor="right"
                open={sidebarOpen}
                onClose={() => setSidebarOpen(false)}
            >
                <div className="sidebar is-flex is-flex-direction-column">
                    <div className="is-flex is-justify-content-space-between">
                        <Typography fontWeight={700} fontSize={20}>
                            {props.filtersLabel}
                        </Typography>
                        <Button
                            className="sidebar__header__button"
                            onClick={() => setSidebarOpen(false)}
                        >
                            <Close />
                        </Button>
                    </div>
                    <div className="sidebar__filters">
                        {props.filterInputs && props.filterInputs.length > 0 && props.filterInputs.map((item, index) => (
                            item.isNested ? (
                                <NestedFilterAccordionDropdown 
                                    key={index}
                                    label={item.label}
                                    filterGroups={item.items}
                                    selectedFilters={props.filters}
                                    onChange={updateFilters}
                                />
                            ) : (
                                <Accordion key={index} className="sidebar__filters__filter">
                                <AccordionSummary
                                    expandIcon={<ArrowIcon />}
                                >
                                    {item.label}
                                </AccordionSummary>
                                {item.items.map((variant, index) => (
                                    <AccordionDetails key={index} className="sidebar__filters__filter__item is-flex is-align-items-center">
                                        <Checkbox
                                            className="sidebar__filters__filter__item__checkbox"
                                            checked={isFilterSelected(item.key, variant.value)}
                                            icon={<CheckboxIcon />}
                                            checkedIcon={<CheckboxCheckedIcon />}
                                            onClick={() => handleOnSidebarFilterSet(item.key, variant.value, variant.label) } />
                                        <Typography>{variant.label}</Typography>
                                    </AccordionDetails>
                                ))}
                            </Accordion>
                        )))}
                    </div>
                    <div className="sidebar__fotter is-flex is-justify-content-space-between">
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
                    </div>
                </div>
            </Drawer>
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