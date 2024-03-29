﻿import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import TitleIcon from '../../elements/misc/TitleIcon'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { TextField, Button } from '@material-ui/core'
import { useHistory } from 'react-router-dom'
import useDebounce from '../../../hooks/useDebounce'
import { useModal } from 'react-modal-hook'
import { fakeFactureListColumns } from '../../elements/table/columns/fakeFactureColumns'
import PrintFakeFacture from '../../elements/dialogs/documents-print/PrintFakeFacture'
import AddIcon from '@material-ui/icons/Add';
import { useLoader } from '../../providers/LoaderProvider'

const DOCUMENT = 'FakeFactures'
const EXPAND = ['Client($select=Id,Name)', 'TypePaiement', 'FakeFactureItems/ArticleFacture']

const FakeFactureClientList = () => {
    const { showLoader } = useLoader();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const [data, setData] = React.useState([]);
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            and: [
                {
                    or: {
                        'Client/Name': {
                            contains: debouncedSearchText
                        },
                        'NumBon': {
                            contains: debouncedSearchText
                        },
                        'Comment': {
                            contains: debouncedSearchText
                        },
                        'ClientName': {
                            contains: debouncedSearchText
                        }
                    }
                }
            ]
        }
    }, [debouncedSearchText]);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const [documentToPrint, setDocumentToPrint] = React.useState(null);
    const history = useHistory();
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => fakeFactureListColumns(),
        []
    );
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintFakeFacture
                onExited={onExited}
                open={open}
                document={documentToPrint}
                onClose={() => {
                    setDocumentToPrint(null);
                    hideModal();
                }}
            />
        )
    }, [documentToPrint]);

    React.useEffect(() => {
        setTitle('Facture')
    }, []);

    const refetchData = () => {
        showLoader(true, true)
        getData(DOCUMENT, {},
            filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
            }).catch((err) => {
                console.log({ err });
            }).finally(() => {
                showLoader()
            })
    }

    const deleteRow = React.useCallback(async (id) => {
        showLoader(true, true)
        const response = await deleteData(DOCUMENT, id);
        console.log({ response });
        if (response) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer la facture sélectionnée !'
            });
        }
        showLoader()
    }, [])

    const updateRow = React.useCallback(async (id) => {
        history.push(`_Facture?FactureId=${id}`);
    }, []);


    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        // setPageIndex(pageIndex);
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            // const endRow = startRow + pageSize
            showLoader(true, true)
            getData(DOCUMENT, {
                $skip: startRow
            }, filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
                setTotalCount(Math.ceil(response.totalItems / pageSize))
            }).catch((err) => {
                console.log({ err });
            }).finally(() => {
                showLoader();
            })
        }
    }, [])

    const print = React.useCallback((document) => {
        setDocumentToPrint(document);
        showModal();
    }, [])

    return (
        <>
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={() => history.push('/_Facture')}
                >
                    Nouvelle facture
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des factures (client)" Icon={DescriptionOutlinedIcon} />
                    <TextField
                        value={searchText}
                        onChange={({ target: { value } }) => {
                            setSearchText(value);
                        }}
                        placeholder="Rechercher..."
                        variant="outlined"
                        size="small"
                    />
                </Box>
                <Box mt={4}>
                    <Table
                        columns={columns}
                        data={data}
                        deleteRow={deleteRow}
                        updateRow={updateRow}
                        print={print}
                        serverPagination
                        totalItems={totalItems}
                        pageCount={pageCount}
                        fetchData={fetchData}
                        filters={filters}
                    />
                </Box>
            </Paper>
        </>
    )
}

export default FakeFactureClientList;
